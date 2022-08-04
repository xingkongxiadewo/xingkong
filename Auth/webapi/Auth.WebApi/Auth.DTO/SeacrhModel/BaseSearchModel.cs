namespace Auth.DTO.SearchModel
{
    public class BaseSearchModel
    {
        public int Limit { private get; set; }
        public int Page { private get; set; }

        public int Skip
        {
            get
            {
                return (Page - 1) * Limit;
            }
        }
        public int Take
        {
            get
            {
                return Limit;
            }
        }
    }
}
