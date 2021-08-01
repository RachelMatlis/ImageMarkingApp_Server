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
    public class DocumentController : ControllerBase
    {
        private IDocument _docService;
        public DocumentController(IDocument docService)
        {
            _docService = docService;
        }

        [HttpPost("Create")]
        public CreateDocResponseDTO Create([FromBody] DocumentDTO doc)
        {
            CreateDocResponseDTO retval = _docService.CreateDocument(doc);
            return retval;
        }

        [HttpPost("GetDocument")]
        public GetDocumentResponseDTO GetDocument(DocumentRequstDTO doc)
        {
            GetDocumentResponseDTO retval = new GetDocumentResponseDTO();
            retval = _docService.GetDocument(doc);
            return retval;
        }

        [HttpPost("GetDocuments")]
        public GetDocumentsResponseDTO GetDocuments(DocumentRequstDTO doc)
        {
            var retval = new GetDocumentsResponseDTO();
            retval = _docService.GetAllDocuments(doc);
            return retval;
        }

        [HttpPost("Remove")]
        public BoolResponseDTO Remove(DocumentRequstDTO doc)
        {
            var retval = _docService.RemoveDocument(doc);
            return retval;

        }
    }
}
