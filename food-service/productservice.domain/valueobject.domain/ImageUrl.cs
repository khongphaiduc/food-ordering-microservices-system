namespace food_service.productservice.domain.valueobject.domain
{
    public class ImageUrl
    {
        public string Value { get; }

        public ImageUrl(string value)
        {
            if (!Uri.IsWellFormedUriString(value, UriKind.Absolute))
                throw new ArgumentException("Invalid image URL");

            Value = value;
        }

    }
}
