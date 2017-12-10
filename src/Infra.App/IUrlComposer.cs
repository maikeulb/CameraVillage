namespace CameraVillage.Domain.Services
{
    public interface IUrlComposer
    {
        string ComposeImageUrl(string urlTemplate);
    }
}
