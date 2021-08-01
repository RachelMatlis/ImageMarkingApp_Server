using ImakeMarkingDTO;
using System;
using System.Collections.Generic;
using System.Text;

namespace ImageMarkingServiceContracs
{
    public interface IDocument
    {
        CreateDocResponseDTO CreateDocument(DocumentDTO doc);
        GetDocumentResponseDTO GetDocument(DocumentRequstDTO doc);
        GetDocumentsResponseDTO GetAllDocuments(DocumentRequstDTO doc);
        BoolResponseDTO RemoveDocument(DocumentRequstDTO doc);
    }
}
