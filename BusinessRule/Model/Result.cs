namespace BusinessRule.Model
{
    public class Result<T>
    {
        public T Data { get; set; }

        public string Messages { get; set; }

        public bool IsValid { get; set; }

    }
}