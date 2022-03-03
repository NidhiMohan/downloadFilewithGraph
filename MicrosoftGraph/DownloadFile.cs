using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Newtonsoft.Json;

namespace DownLoadFileGraph.MicrosoftGraph
{
    public class User
    {
        public string email { get; set; }
        public string id { get; set; }
        public string displayName { get; set; }
    }

    public class CreatedBy
    {
        public User user { get; set; }
    }

    public class LastModifiedBy
    {
        public User user { get; set; }
    }

    public class ParentReference
    {
        public string driveId { get; set; }
        public string driveType { get; set; }
        public string id { get; set; }
        public string path { get; set; }
    }

    public class Hashes
    {
        public string quickXorHash { get; set; }
    }

    public class File
    {
        public string mimeType { get; set; }
        public Hashes hashes { get; set; }
    }

    public class FileSystemInfo
    {
        public DateTime createdDateTime { get; set; }
        public DateTime lastModifiedDateTime { get; set; }
    }

    public class Image
    {
    }

    public class DownloadURL
    {
        [JsonProperty("@odata.context")]
        public string OdataContext { get; set; }

        [JsonProperty("@microsoft.graph.downloadUrl")]
        public string MicrosoftGraphDownloadUrl { get; set; }
        public DateTime createdDateTime { get; set; }
        public string eTag { get; set; }
        public string id { get; set; }
        public DateTime lastModifiedDateTime { get; set; }
        public string name { get; set; }
        public string webUrl { get; set; }
        public string cTag { get; set; }
        public int size { get; set; }
        public CreatedBy createdBy { get; set; }
        public LastModifiedBy lastModifiedBy { get; set; }
        public ParentReference parentReference { get; set; }
        public File file { get; set; }
        public FileSystemInfo fileSystemInfo { get; set; }
        public Image image { get; set; }
    }
}
