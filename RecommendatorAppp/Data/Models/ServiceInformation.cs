using System.ComponentModel;

namespace RecommendatorAppp.Models
{
    public class ServiceInformation
    {
        public int Id { get; set; }

        [DisplayName("Select Service")]
        public int ServiceId { get; set; }

        [DisplayName("Select Information")]
        public int InformationId { get; set; }

        public virtual Information Informations { get; set; }
        public virtual Services Services { get; set; }
        public object Information { get; internal set; }
        public object Name { get; internal set; }
        public int? ServicesId { get; internal set; }
    }
}