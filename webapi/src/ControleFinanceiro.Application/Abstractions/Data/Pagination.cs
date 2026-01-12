using FluentResults;

namespace ControleFinanceiro.Application.Abstractions.Data;

public abstract class PaginationError(string message) : Error(message);

public class InvalidPageNumberError(int page)
    : PaginationError($"O número da página '{page}' é inválido. Deve ser >= 0.")
{
    public int Page { get; } = page;
}

public class InvalidPageSizeError(int pageSize)
    : PaginationError($"O tamanho da página '{pageSize}' é inválido. Deve ser >= 1.")
{
    public int PageSize { get; } = pageSize;
}

public interface IPagination
{
    int Pagina { get; }
    int TamanhoPagina { get; }
}

public record Pagination(int Pagina, int TamanhoPagina) : IPagination
{
    public int Pagina { get; } = Pagina;
    public int TamanhoPagina { get; } = TamanhoPagina;

    public static Result Validate(IPagination pagination)
    {
        var errors = new List<PaginationError>();

        if (pagination.Pagina < 0)
            errors.Add(new InvalidPageNumberError(pagination.Pagina));
        if (pagination.TamanhoPagina < 1)
            errors.Add(new InvalidPageSizeError(pagination.TamanhoPagina));

        if (errors.Any())
            return Result.Fail(errors);

        return Result.Ok();
    }
}

public record PaginatedResponse<T>
{
    public int Page { get; init; }
    public int PageSize { get; init; }
    public int TotalPages { get; init; }
    public int Total { get; init; }
    public IReadOnlyList<T> Items { get; init; }
    public bool HasPreviousPage => Page > 0;
    public bool HasNextPage => TotalPages - (Page + 1) > 0;

    public PaginatedResponse(IPagination pagination, List<T> items, int total)
    {
        if (pagination.Pagina < 0)
            throw new ArgumentOutOfRangeException(nameof(pagination.Pagina),
                $"Page number '{pagination.Pagina}' is invalid. Must be >= 0.");

        if (pagination.TamanhoPagina < 1)
            throw new ArgumentOutOfRangeException(nameof(pagination.TamanhoPagina),
                $"Page size '{pagination.TamanhoPagina}' is invalid. Must be >= 1.");

        if (total < 0)
            throw new ArgumentOutOfRangeException(nameof(total),
                $"Total '{total}' is invalid. Must be >= 0.");

        Page = pagination.Pagina;
        PageSize = pagination.TamanhoPagina;
        Total = total;
        TotalPages = (int)Math.Ceiling((double)Total / PageSize);
        Items = items?.AsReadOnly() ?? new List<T>().AsReadOnly();
    }
}
