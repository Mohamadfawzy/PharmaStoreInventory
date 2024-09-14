using DataAccess.DomainModel;
using System.IO.Enumeration;
using System.Xml.Linq;

namespace DataAccess.Services;

public class XmlFileHandler
{
    readonly string path;
    readonly string emlementName = "Branch";
    readonly string rootName = "Branchs";
    public XmlFileHandler(string path)
    {
        this.path = path;
        Initial();
    }

    public async void Initial()
    {
        if (File.Exists(path))
            return;
        await Task.Run(() =>
        {
            XDocument xDocument = new XDocument(new XElement(rootName));
            xDocument.Save(path);
        });
    }

    public async Task<Result> Add(BranchModel branch)
    {
        try
        {
            //if (await IsIpAdrressExist(branch.IpAddress))
            //    return Result.Failure(ErrorCode.ItemIsExist);

            var doc = XDocument.Load(path);
            if (doc != null)
            {
                var root = doc.Element(rootName);
                if (root != null)
                {
                    // Add a new 'Brach' element with child elements and attributes
                    root.Add(new XElement(emlementName,
                                new XAttribute(nameof(BranchModel.Id), branch.Id),
                                new XElement(nameof(BranchModel.BrachName), branch.BrachName),
                                new XElement(nameof(BranchModel.Username), branch.Username),
                                new XElement(nameof(BranchModel.Password), branch.Password),
                                new XElement(nameof(BranchModel.Telephone), branch.Telephone),
                                new XElement(nameof(BranchModel.IpAddress), branch.IpAddress),
                                new XElement(nameof(BranchModel.Port), branch.Port),
                                new XElement(nameof(BranchModel.UserId), branch.UserId)

                    ));
                    root.Save(path);
                }
            }
            return Result.Success();
        }
        catch (Exception ex)
        {
            return Result.Failure(ErrorCode.ExceptionError, ex.Message);
        }
    }

    public async Task<Result> RemoveById(string id)
    {
        try
        {
            await Task.Run(() =>
            {
                var doc = XDocument.Load(path);
                doc.Descendants(emlementName)
                   .Where(x => x.Attribute("Id")!.Value == id)
                   .Remove();
                doc.Save(path);
            });

            return Result.Success();
        }
        catch (Exception ex)
        {
            return Result.Failure(ErrorCode.ExceptionError, ex.Message);
        }
    }

    public Task<bool> IsIpAdrressExist(string ipAddress)
    {
        var doc = XDocument.Load(path);
        var existing = doc.Descendants(emlementName).Any(x => x.Element("IpAddress")!.Value == ipAddress);
        return Task.FromResult(existing);
    }

    public async Task<List<BranchModel>?> All()
    {
        try
        {
            return await Task.Run(() =>
             {
                 var students = XDocument.Load(path)?
                 .Descendants(emlementName)
                 .Select(x => new BranchModel()
                 {
                     Id = Guid.Parse(x.Attribute("Id")?.Value ?? Guid.Empty.ToString()),
                     BrachName = x.Element("BrachName")?.Value ?? string.Empty,
                     Username = x.Element("Username")?.Value ?? string.Empty,
                     Password = x.Element("Password")?.Value ?? string.Empty,
                     Telephone = x.Element("Telephone")?.Value ?? string.Empty,
                     IpAddress = x.Element("IpAddress")?.Value ?? string.Empty,
                     Port = x.Element("Port")?.Value ?? string.Empty,
                     UserId = int.Parse(x.Element("UserId")?.Value ?? "0"),
                 }).ToList();
                 return students;
             });
        }

        catch
        {
            return null;
        }


    }
}
