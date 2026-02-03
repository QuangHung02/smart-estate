using SmartEstate.App.Common.Abstractions;

namespace SmartEstate.Api.Integrations;

public sealed class LocalFileStorage : IFileStorage
{
    private readonly IWebHostEnvironment _env;

    public LocalFileStorage(IWebHostEnvironment env)
    {
        _env = env;
    }

    public async Task<string> UploadAsync(Stream content, string fileName, string contentType, CancellationToken ct = default)
    {
        var uploadsRoot = Path.Combine(_env.ContentRootPath, "uploads");
        Directory.CreateDirectory(uploadsRoot);

        var safeExt = Path.GetExtension(fileName);
        if (string.IsNullOrWhiteSpace(safeExt)) safeExt = ".bin";

        var newName = $"{Guid.NewGuid():N}{safeExt}";
        var path = Path.Combine(uploadsRoot, newName);

        await using var fs = new FileStream(path, FileMode.CreateNew, FileAccess.Write, FileShare.None);
        await content.CopyToAsync(fs, ct);

        // simplest: serve via /uploads/{file}
        return $"/uploads/{newName}";
    }
}
