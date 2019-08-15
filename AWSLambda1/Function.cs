using System.Collections.Generic;
using System.Linq;
using System.Net;
using Amazon.Lambda.Core;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
// Assembly attribute to enable the Lambda function's JSON input to be converted into a .NET class.
[assembly: LambdaSerializer(typeof(Amazon.Lambda.Serialization.Json.JsonSerializer))]
namespace AWSLambda1
{
    public class Function
    {
        public object FunctionHandler(string input, ILambdaContext context)
        {
            int i;
            if (int.TryParse(input, out i))
            {
                string albumsString;
                string photosString;
                using (WebClient wc = new WebClient())
                {
                    albumsString = wc.DownloadString("http://jsonplaceholder.typicode.com/albums");
                    photosString = wc.DownloadString("http://jsonplaceholder.typicode.com/photos");
                }
                IEnumerable<dynamic> albums = JsonConvert.DeserializeObject<IEnumerable<dynamic>>(albumsString);
                IEnumerable<dynamic> photos = JsonConvert.DeserializeObject<IEnumerable<dynamic>>(photosString);
                if (albums != null && photos != null)
                {
                    var filledObjects = albums
                        .Where(alb => alb.userId == i || i == 0)
                        .Join(photos, alb => alb.id.Value, pho => pho.albumId.Value, (alb, pho) => new { album = alb, photo = pho })
                        .GroupBy(o => o.album)
                        .Select(g =>
                        {
                            var albumToFill = g.Key;
                            albumToFill.Photos = new JArray(g.Select(o => o.photo));
                            return albumToFill;
                        });
                    return filledObjects;
                }
            }
            return null;
        }
    }
}
