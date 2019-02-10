namespace WpfSampleApp
{
    // Customer DTO (no need to use serializable attributes because of XML serialization)
    public class Customer
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
    }
}
