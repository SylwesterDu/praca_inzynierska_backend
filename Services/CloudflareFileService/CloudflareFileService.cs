using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Amazon;
using Amazon.Runtime;
using Amazon.S3;
using Amazon.S3.Model;

namespace praca_inzynierska_backend.Services.CloudflareFileService
{
    public class CloudflareFileService : ICloudflareFileService
    {
        private readonly IConfiguration _configuration;
        private readonly AWSCredentials _credentials;
        AmazonS3Client _client;

        public CloudflareFileService(IConfiguration configuration)
        {
            _configuration = configuration;
            _credentials = new BasicAWSCredentials(
                _configuration["R2:AccessKey"],
                _configuration["R2:SecretKey"]
            );

            string accountId = _configuration["R2:AccountId"];
            _client = new AmazonS3Client(
                _credentials,
                new AmazonS3Config { ServiceURL = $"https://{accountId}.r2.cloudflarestorage.com", }
            );

            AWSConfigsS3.UseSignatureVersion4 = true;
        }

        public string GetFileUrl(string key)
        {
            GetPreSignedUrlRequest request = new GetPreSignedUrlRequest();
            request.BucketName = _configuration["R2:BucketName"];
            request.Key = key;
            request.Protocol = Protocol.HTTPS;
            request.Expires = DateTime.Now.AddDays(1);
            string url = _client.GetPreSignedURL(request);
            return url;
        }

        public async Task<string> UploadFile(Guid artworkId, IFormFile file)
        {
            string key = artworkId.ToString() + "_" + file.FileName;

            try
            {
                PutObjectRequest putRequest = new PutObjectRequest
                {
                    BucketName = _configuration["R2:BucketName"],
                    Key = key,
                    InputStream = file.OpenReadStream(),
                    ContentType = file.ContentType,
                    DisablePayloadSigning = true,
                };
                PutObjectResponse response = await _client.PutObjectAsync(putRequest);
                return key;
            }
            catch (AmazonS3Exception amazonS3Exception)
            {
                if (
                    amazonS3Exception.ErrorCode != null
                    && (
                        amazonS3Exception.ErrorCode.Equals("InvalidAccessKeyId")
                        || amazonS3Exception.ErrorCode.Equals("InvalidSecurity")
                    )
                )
                {
                    throw new Exception("Check the provided AWS Credentials.");
                }
                else
                {
                    throw new Exception("Error occurred: " + amazonS3Exception.Message);
                }
            }
        }
    }
}
