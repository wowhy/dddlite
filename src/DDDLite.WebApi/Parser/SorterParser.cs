namespace DDDLite.WebApi.Parser
{
    using DDDLite.Querying;
    using DDDLite.Specifications;

    public class SorterParser<TAggregateRoot>
        where TAggregateRoot : class
    {
        public SorterParser()
        {
        }

        public SortSpecification<TAggregateRoot> Parse(string sorter)
        {
            var sortSpecification = new SortSpecification<TAggregateRoot>();
            if (!string.IsNullOrWhiteSpace(sorter))
            {
                sorter = sorter.Trim();
                var sorts = sorter.Split(',');
                foreach (var sort in sorts)
                {
                    var tokens = sort.Trim().Split(' ');
                    if (tokens.Length == 1)
                    {
                        sortSpecification.Add(tokens[0].Trim(), SortDirection.Asc);
                    }
                    else if (tokens.Length == 2)
                    {
                        sortSpecification.Add(tokens[0].Trim(), ParseDirection(tokens[1]));
                    }
                    else
                    {
                        throw new Exception.SorterParseException();
                    }
                }
            }

            return sortSpecification;
        }

        private static SortDirection ParseDirection(string token)
        {
            if (token == null)
            {
                throw new Exception.SorterParseException();
            }

            switch (token.Trim().ToLower())
            {
                case "d":
                case "desc":
                    return SortDirection.Desc;

                case "a":
                case "asc":
                    return SortDirection.Asc;
                default:
                    throw new Exception.SorterParseException();
            }
        }
    }
}