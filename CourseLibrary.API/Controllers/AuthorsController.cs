using System;
using System.Collections;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Text.Json;
using AutoMapper;
using CourseLibrary.API.Entities;
using CourseLibrary.API.Helpers;
using CourseLibrary.API.Models;
using CourseLibrary.API.ResourceParameters;
using CourseLibrary.API.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Primitives;
using Microsoft.Net.Http.Headers;

namespace CourseLibrary.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthorsController : ControllerBase
    {
        private readonly ICourseLibraryRepository _courseLibraryRepository;
        private readonly IMapper _mapper;
        private readonly IPropertyMappingService _propertyMappingService;
        private readonly IPropertyCheckerService _propertyCheckerService;

        public AuthorsController(ICourseLibraryRepository courseLibraryRepository, IMapper mapper, IPropertyMappingService propertyMappingService, IPropertyCheckerService propertyCheckerService)
        {
            _courseLibraryRepository = courseLibraryRepository ??
                throw new ArgumentNullException(nameof(courseLibraryRepository));
            _mapper = mapper ??
                throw new ArgumentNullException(nameof(mapper));
            _propertyMappingService = propertyMappingService ??
                throw new ArgumentNullException(nameof(propertyMappingService));
            _propertyCheckerService = propertyCheckerService ??
                throw new ArgumentNullException(nameof(propertyCheckerService));
        }

        [Produces("application/json",
            "application/vnd.shaggy.hateoas+json",
            "application/vnd.shaggy.author.full+json",
            "application/vnd.shaggy.author.full.hateoas+json",
            "application/vnd.shaggy.author.friendly+json",
            "application/vnd.shaggy.author.friendly.hateoas+json")]
        [HttpGet(Name = "GetAuthors")]
        [HttpHead]
        public IActionResult GetAuthors(
            [FromQuery] AuthorsResourceParameters authorsResourceParameters,
            [FromHeader(Name = "Accept")] string mediaType)
        {
            if (!MediaTypeHeaderValue.TryParse(mediaType, out MediaTypeHeaderValue parsedMediaType))
            {
                return BadRequest();
            }

            if (!_propertyMappingService.ValidMappingExistsFor<AuthorDto, Author>
                (authorsResourceParameters.OrderBy))
            {
                return BadRequest();
            }

            if (!_propertyCheckerService.TypeHasProperties<AuthorDto>(authorsResourceParameters.Fields))
            {
                return BadRequest();
            }

            var authorsFromRepository = _courseLibraryRepository.GetAuthors(authorsResourceParameters);

            var paginationMetadata = new
            {
                totalCount = authorsFromRepository.TotalCount,
                pageSize = authorsFromRepository.PageSize,
                currentPage = authorsFromRepository.CurrentPage,
                totalPages = authorsFromRepository.TotalPages
            };

            Response.Headers.Add("X-Pagination", JsonSerializer.Serialize(paginationMetadata));

            var includeLinks = ShouldLinksBeIncluded(parsedMediaType);

            IEnumerable<LinkDto> links = new List<LinkDto>();

            var primaryMediaType = GetPrimaryMediaType(includeLinks, parsedMediaType);

            var shapedAuthors =
                (primaryMediaType == "vnd.shaggy.author.full")
                ? MapAndShapeResource<IEnumerable<Author>, IEnumerable<AuthorFullDto>>
                    (authorsFromRepository, authorsResourceParameters.Fields)
                : MapAndShapeResource<IEnumerable<Author>, IEnumerable<AuthorDto>>
                    (authorsFromRepository, authorsResourceParameters.Fields);

            if (includeLinks)
            {
                links = CreateLinksForAuthors(
                    authorsResourceParameters,
                    authorsFromRepository.HasNext,
                    authorsFromRepository.HasPrevious);

                var shapedAuthorsWithLinks = shapedAuthors.Select(author =>
                {
                    var authorAsDictionary = author as IDictionary<string, object>;

                    var authorLinks = CreateLinksForAuthor((Guid)authorAsDictionary["Id"], null);
                    authorAsDictionary.Add("links", authorLinks);

                    return author;
                });

                var linkedCollectionResource = new
                {
                    value = shapedAuthorsWithLinks,
                    links
                };

                return Ok(linkedCollectionResource);
            }

            return Ok(shapedAuthors);
        }

        [Produces("application/json",
            "application/vnd.shaggy.hateoas+json",
            "application/vnd.shaggy.author.full+json",
            "application/vnd.shaggy.author.full.hateoas+json",
            "application/vnd.shaggy.author.friendly+json",
            "application/vnd.shaggy.author.friendly.hateoas+json")]
        [HttpGet("{authorId:guid}", Name = "GetAuthor")]
        public IActionResult GetAuthor(Guid authorId, string fields,
            [FromHeader(Name = "Accept")] string mediaType)
        {
            if (!MediaTypeHeaderValue.TryParse(mediaType, out MediaTypeHeaderValue parsedMediaType))
            {
                return BadRequest();
            }

            if (!_propertyCheckerService.TypeHasProperties<AuthorDto>(fields))
            {
                return BadRequest();
            }

            var authorFromRepository = _courseLibraryRepository.GetAuthor(authorId);

            if (authorFromRepository == null)
            {
                return NotFound();

            }

            var includeLinks = ShouldLinksBeIncluded(parsedMediaType);

            IEnumerable<LinkDto> links = new List<LinkDto>();

            if (includeLinks)
            {
                links = CreateLinksForAuthor(authorId, fields);
            }

            var primaryMediaType = GetPrimaryMediaType(includeLinks, parsedMediaType);

            var authorToReturn =
                (primaryMediaType == "vnd.shaggy.author.full")
                ? MapShapeAndCastResource<Author, AuthorFullDto, IDictionary<string, object>>
                    (authorFromRepository, fields)
                : MapShapeAndCastResource<Author, AuthorDto, IDictionary<string, object>>
                    (authorFromRepository, fields);

            if (includeLinks)
            {
                authorToReturn.Add("links", links);
            }

            return Ok(authorToReturn);
        }

        [HttpPost(Name = "CreateAuthor")]
        public ActionResult<AuthorDto> CreateAuthor(AuthorForCreationDto author)
        {
            var authorEntity = _mapper.Map<Author>(author);

            _courseLibraryRepository.AddAuthor(authorEntity);
            _courseLibraryRepository.Save();

            var authorToReturn = _mapper.Map<AuthorDto>(authorEntity);

            var links = CreateLinksForAuthor(authorToReturn.Id, null);

            var linkedResourceToReturn = 
                authorToReturn
                    .ShapeData(null)
                    as IDictionary<string, object>;

            linkedResourceToReturn.Add("links", links);

            return CreatedAtRoute("GetAuthor",
                new { authorId = linkedResourceToReturn["Id"] },
                linkedResourceToReturn);
        }

        [HttpOptions]
        public IActionResult GetAuthorsOptions()
        {
            Response.Headers.Add("Allow", "GET, OPTIONS, POST");
            return Ok();
        }

        [HttpDelete("{authorId}", Name = "DeleteAuthor")]
        public IActionResult DeleteAuthor(Guid authorId)
        {
            var authorFromRepo = _courseLibraryRepository.GetAuthor(authorId);

            if (authorFromRepo == null)
            {
                return NotFound();
            }

            _courseLibraryRepository.DeleteAuthor(authorFromRepo);
            _courseLibraryRepository.Save();

            return NoContent();
        }

        private bool ShouldLinksBeIncluded(MediaTypeHeaderValue parsedMediaType)
            => parsedMediaType
                    .SubTypeWithoutSuffix
                    .EndsWith("hateoas", StringComparison.InvariantCultureIgnoreCase);

        private StringSegment GetPrimaryMediaType(bool includeLinks, MediaTypeHeaderValue parsedMediaType)
            => includeLinks
                    ? parsedMediaType
                        .SubTypeWithoutSuffix
                        .Substring(0, parsedMediaType.SubTypeWithoutSuffix.Length - 8)
                    : parsedMediaType.SubTypeWithoutSuffix;

        private IEnumerable<ExpandoObject> MapAndShapeResource<TCurrentType, TMappingType>
            (TCurrentType resource, string fields) where TMappingType : IEnumerable
            => _mapper
                    .Map<TMappingType>(resource)
                    .ShapeData(fields);

        private TCastingType MapShapeAndCastResource<TCurrentType, TMappingType, TCastingType>
            (TCurrentType resource, string fields) where TCastingType : class
            => _mapper
                    .Map<TMappingType>(resource)
                    .ShapeData(fields)
                    as TCastingType;

        private IEnumerable<LinkDto> CreateLinksForAuthor(Guid authorId, string fields)
        {
            var links = new List<LinkDto>();

            if (string.IsNullOrWhiteSpace(fields))
            {
                links.Add(
                    new LinkDto(Url.Link("GetAuthor", new { authorId }),
                    "self",
                    "GET"));
            }
            else
            {
                links.Add(
                    new LinkDto(Url.Link("GetAuthor", new { authorId, fields }),
                    "self",
                    "GET"));
            }

            links.Add(
                new LinkDto(Url.Link("DeleteAuthor", new { authorId }),
                    "delete_author",
                    "DELETE"));

            links.Add(
                new LinkDto(Url.Link("CreateCourseForAuthor", new { authorId }),
                    "create_course_for_author",
                    "POST"));

            links.Add(
                new LinkDto(Url.Link("GetCoursesForAuthor", new { authorId }),
                    "get_courses_for_author",
                    "GET"));

            return links;
        }

        private IEnumerable<LinkDto> CreateLinksForAuthors(
            AuthorsResourceParameters authorsResourceParameters,
            bool hasNext,
            bool hasPrevious)
        {
            var links = new List<LinkDto>();

            links.Add(
                new LinkDto(CreateAuthorsResourceUri(authorsResourceParameters, ResourceUriType.Current),
                "self",
                "GET"));

            if (hasNext)
            {
                links.Add(
                    new LinkDto(CreateAuthorsResourceUri(authorsResourceParameters, ResourceUriType.NextPage),
                    "nextPage",
                    "GET"));
            }

            if (hasPrevious)
            {
                links.Add(
                    new LinkDto(CreateAuthorsResourceUri(authorsResourceParameters, ResourceUriType.PreviousPage),
                    "previousPage",
                    "GET"));
            }

            return links;
        }

        private string CreateAuthorsResourceUri(
            AuthorsResourceParameters authorsResourceParameters,
            ResourceUriType type)
        {
            var pageNumber = type switch
            {
                ResourceUriType.PreviousPage => authorsResourceParameters.PageNumber - 1,
                ResourceUriType.NextPage => authorsResourceParameters.PageNumber + 1,
                ResourceUriType.Current => authorsResourceParameters.PageNumber,
                _ => authorsResourceParameters.PageNumber
            };

            return Url.Link("GetAuthors",
                new
                {
                    fields = authorsResourceParameters.Fields,
                    orderBy = authorsResourceParameters.OrderBy,
                    pageNumber,
                    pageSize = authorsResourceParameters.PageSize,
                    mainCategory = authorsResourceParameters.MainCategory,
                    searchQuery = authorsResourceParameters.SearchQuery
                });
        }
    }
}
