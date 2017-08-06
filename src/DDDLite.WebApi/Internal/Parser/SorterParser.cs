namespace DDDLite.WebApi.Internal.Parser
{
    using DDDLite.Querying;
    using DDDLite.Specifications;

    public class SorterParser<TAggregateRoot>
        where TAggregateRoot: class
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
                foreach(var sort in sorts)
                {
                    var tokens = sort.Trim().Split(' ');
                    if (tokens.Length == 1) 
                    {
                        sortSpecification.Add(tokens[0].Trim(), SortDirection.Asc);
                    } else if (tokens.Length == 2) 
                    {
                        sortSpecification.Add(tokens[0].Trim(), ParseDirection(tokens[1]));
                    } else 
                    {
                        throw new Exception.SorterParseException();
                    }
                }
            }

            return sortSpecification;
        }

        private static SortDirection ParseDirection(string token)
        {
            switch(token?.Trim())
            {
                case "d":
                    return SortDirection.Desc;

                case "a":
                    return SortDirection.Asc;
            }

            throw new Exception.SorterParseException();
        }
    }
}