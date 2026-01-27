using food_service.ProductService.Application.Interface;
using Minio;
using Minio.DataModel.Args;

namespace food_service.ProductService.Infastructure.MinIO
{
    public class MinIOFood : IMinIOFood
    {
        private readonly IConfiguration _configuration;
        private readonly IMinioClient _clientMinIO;

        public MinIOFood(IConfiguration configuration, IMinioClient minioClient)
        {
            _configuration = configuration;
            _clientMinIO = minioClient;
        }


        public async Task DeleteAsync(string objectName)
        {
            await _clientMinIO.RemoveObjectAsync(
            new RemoveObjectArgs()
                .WithBucket(_configuration["Minio:Bucket"])
                .WithObject(objectName));
        }


        public async Task<string> UploadAsync(IFormFile file)
        {
            var objectName = $"{Guid.NewGuid()}{Path.GetExtension(file.FileName)}";   //GetExtention lấy đuôi file

            using var stream = file.OpenReadStream();

            var found = await _clientMinIO.BucketExistsAsync(new BucketExistsArgs().WithBucket(_configuration["Minio:Bucket"]));

            if (!found)
            {
                await _clientMinIO.MakeBucketAsync(new MakeBucketArgs().WithBucket(_configuration["Minio:Bucket"]));
            }

            await _clientMinIO.PutObjectAsync(
                new PutObjectArgs()
                    .WithBucket(_configuration["Minio:Bucket"])
                    .WithObject(objectName)
                    .WithStreamData(stream)
                    .WithObjectSize(stream.Length)
                    .WithContentType(file.ContentType)
            );
            return objectName;
        }
    }
}
