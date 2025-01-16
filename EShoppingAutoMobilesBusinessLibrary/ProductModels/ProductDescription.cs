using Newtonsoft.Json;
namespace EShoppingAutoMobilesBusinessLibrary.ProductModels
{
    public class ProductDescription
    {

        [JsonProperty("Product Id")]
        public int ProductID { get; set; }
        [JsonProperty("Processor")]
        public string Processor { get; set; }
        [JsonProperty("Operating System")]
        public string OperatingSystem { get; set; }
        [JsonProperty("Graphic Card")]
        public string GraphicCard { get; set; }
        [JsonProperty("Memory")]
        public string Memory { get; set; }
        [JsonProperty("Storage")]
        public string Storage { get; set; }
        [JsonProperty("Color")]
        public string Color { get; set; }
    }
}
