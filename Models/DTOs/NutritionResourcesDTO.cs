namespace FitnessPartner.Models.DTOs
{
    public class NutritionResourcesDTO
    {
        public int ResourceId { get; set; }
        public string ResourceTitle { get; set; } = string.Empty;
        public string ResourceType { get; set; } = string.Empty;
        public string Content { get; set; } = string.Empty;

        public NutritionResourcesDTO(int resourceId, string resourceTitle, string resourceType, string content)
        {
            ResourceId = resourceId;
            ResourceTitle = resourceTitle;
            ResourceType = resourceType;
            Content = content;
        }
    }
}