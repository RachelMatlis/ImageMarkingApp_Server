using ImageMarkingServiceContracs;
using ImakeMarkingDTO;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ImageMarkingApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SharingController : ControllerBase
    {
        private ISharingService _sharingService;
        public SharingController(ISharingService sharingService)
        {
            _sharingService = sharingService;
        }

        [HttpPost("Create")]
        public BoolResponseDTO Create([FromBody] ShareDocDTO share)
        {
            BoolResponseDTO retval = _sharingService.ShareOnDocument(share);
            return retval;
        }

        [HttpPost("RemoveShare")]
        public BoolResponseDTO RemoveShare([FromBody] ShareDocDTO share)
        {
            BoolResponseDTO retval = _sharingService.RemoveShareOnDocument(share);
            return retval;
        }

        [HttpPost("GetSharedDocs")]
        public GetSharedDocumentsResponseDTO GetSharedDocs([FromBody] ShareDocDTO share)
        {
            GetSharedDocumentsResponseDTO retval = _sharingService.GetSharedDocuments(share.SharedUserID);
            return retval;
        }
    }
}
