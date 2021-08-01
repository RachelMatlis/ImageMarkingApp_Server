using ImakeMarkingDTO;
using System;
using System.Collections.Generic;
using System.Text;

namespace ImageMarkingServiceContracs
{
    public interface ISharingService
    {
        BoolResponseDTO ShareOnDocument(ShareDocDTO share);
        BoolResponseDTO RemoveShareOnDocument(ShareDocDTO share);
        GetSharedDocumentsResponseDTO GetSharedDocuments(string userID);
    }
}
