using Ardalis.GuardClauses;
using MediatR;
using Studens.Application.Contracts.Dtos;
using Studens.MediatR;

namespace Studens.MvcNet6.WebUI.MediatR.Categories
{
	public class CategoryLocaleDto
	{
		public string Name { get; set; }
	}

	public class CategoryCreateDto :
	ITranslatableEntityDto<CategoryLocaleDto>,
	ICommand<Unit>
	{
		public CategoryCreateDto()
		{
			Translations = new List<CategoryLocaleDto>();
		}

		public IList<CategoryLocaleDto> Translations { get; set; }
	}

	public class CategoryCreateHandler : IRequestHandler<CategoryCreateDto, Unit>
	{
		public async Task<Unit> Handle(CategoryCreateDto request, CancellationToken cancellationToken)
		{
			Guard.Against.Null(request, nameof(request));

			

			return Unit.Value;
		}
	}
}