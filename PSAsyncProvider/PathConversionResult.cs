namespace PSAsyncProvider
{
    public class PathConversionResult
    {
        public static PathConversionResult Converted(
            string updatedPath,
            string updatedFilter)
        {
            return new PathConversionResult(
                true, updatedPath, updatedFilter);
        }

        public static PathConversionResult NotAltered(
            string originalPath,
            string originalFilter)
        {
            return new PathConversionResult(
                false, originalPath, originalFilter);
        }

        private PathConversionResult(
            bool altered,
            string path,
            string filter)
        {
            this.Altered = altered;
            this.Path = path;
            this.Filter = filter;
        }

        public bool Altered { get; }

        public string Path { get; }

        public string Filter { get; }
    }
}
