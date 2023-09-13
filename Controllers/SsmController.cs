using Amazon.SimpleSystemsManagement.Model;
using Amazon.SimpleSystemsManagement;
using Microsoft.AspNetCore.Mvc;
using System.Text;

namespace AwsParameterStorePoc.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SsmController : ControllerBase
    {
        private readonly AmazonSimpleSystemsManagementClient _ssmClient;
        public SsmController()
        {
            _ssmClient = new AmazonSimpleSystemsManagementClient(Amazon.RegionEndpoint.USEast1);
        }

        [HttpGet("[action]/{key}")]
        public async Task<IActionResult> GetParams([FromRoute] string key)
        {
            StringBuilder stringBuilder= new StringBuilder();
            stringBuilder.AppendLine($"Params:{key} : Initiated");
            try
            {
                string res = await GetParameter(key);
                stringBuilder.AppendLine(res);
            }
            catch (Exception ex)
            {
               stringBuilder.AppendLine(ex.ToString());
                if(ex.InnerException is not null)
                {
                    stringBuilder.AppendLine(ex.InnerException.ToString());
                }
            }
            stringBuilder.AppendLine($"Params:{key} : Completed");
            return Ok(stringBuilder.ToString());
        }

        public async Task<string> GetParameter(string key)
        {
            GetParameterRequest getRequest = new()
            {
                Name = key,
                WithDecryption = true // Set to true if the parameter value is encrypted
            };
            GetParameterResponse getResponse = await _ssmClient.GetParameterAsync(getRequest);
            return getResponse.Parameter.Value;
          
        }
    }
}
