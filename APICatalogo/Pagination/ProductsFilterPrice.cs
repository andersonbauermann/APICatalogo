namespace APICatalogo.Pagination;

public class ProductsFilterPrice : PaginationParameters
{
    public decimal? Price { get; set; }
    public CriterionPriceEnum CriterionPrice { get; set; }

    public enum CriterionPriceEnum
    {
        BIGGER_THAN = 1,
        SMALLER_THAN,
        EQUAL
    }
}