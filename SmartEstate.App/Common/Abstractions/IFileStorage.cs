namespace SmartEstate.App.Common.Abstractions;

public interface IFileStorage
{
    Task<string> UploadAsync(Stream content, string fileName, string contentType, CancellationToken ct = default);
}
