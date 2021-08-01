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
    public class MarkerController : ControllerBase
    {
        IMarkerService _markerService;
        public MarkerController(IMarkerService markerService)
        {
            _markerService = markerService;
        }

        [HttpPost("Create")]
        public CreateMarkerResponseDTO Create([FromBody] MarkerDTO marker)
        {
            var retval = _markerService.CreateMarker(marker);
            return retval;
        }

        [HttpPost("Remove")]
        public BoolResponseDTO Remove([FromBody] MarkerDTO marker)
        {
            var retval = _markerService.RemoveMarker(marker.MarkerID);
            return retval;
        }

        [HttpPost("GetAllMarkers")]
        public GetMarkersResponseDTO GetAllMarkers([FromBody] DocumentDTO doc)
        {
            var retval = _markerService.GetMarkers(doc.DocumentId);
            return retval;
        }

        [HttpPost("Update")]
        public BoolResponseDTO Update([FromBody] MarkerDTO marker)
        {
            var retval = _markerService.UpdateMarker(marker);
            return retval;
        }
    }
}
