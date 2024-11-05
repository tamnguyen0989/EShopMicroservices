namespace Catalog.API.Products.CreateProduct
{
    public record CreateProductCommand(string Name, List<string> Category, string Description, string ImageFile, decimal Price) : ICommand<CreateProductResult>;
    public record CreateProductResult(Guid Id);
    public class CreateProductCommandValidatior: AbstractValidator<CreateProductCommand>
    {
        public CreateProductCommandValidatior() {
            RuleFor(c => c.Name).NotEmpty().WithMessage("Name is required");
            RuleFor(c => c.Category).NotEmpty().WithMessage("Category is required");
            RuleFor(c => c.ImageFile).NotEmpty().WithMessage("ImageFile is required");
            RuleFor(c => c.Price).GreaterThan(0).WithMessage("Price must be greater than 0");
        }
    }
    internal class CreateProductCommandHandler(IDocumentSession session)
        : ICommandHandler<CreateProductCommand, CreateProductResult>
    {
        public async Task<CreateProductResult> Handle(CreateProductCommand command, CancellationToken cancellationToken)
        {
            var product = new Product
            {
                Name = command.Name,
                Category = command.Category,
                Description = command.Description,
                ImageFile = command.ImageFile,
                Price = command.Price
            };

            session.Store(product);
            await session.SaveChangesAsync(cancellationToken);

            return new CreateProductResult(Guid.NewGuid());
        }
    }
}
