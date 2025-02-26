namespace Baustellen.App.Shared.Models.ViewModels
{
    public class ExternalLinkViewDto
    {
        public Guid Id { get; set; }
        public string Link { get; set; }
        public LinkTypeEnum Type { get; set; }
    }
}
