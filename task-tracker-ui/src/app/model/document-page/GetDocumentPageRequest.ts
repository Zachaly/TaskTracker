import PagedRequest, { mapPagedRequest } from "../PagedRequest";

export default interface GetDocumentPageRequest extends PagedRequest {
    documentId?: number
}

export const mapGetDocumentPageRequest = (request: GetDocumentPageRequest) => {
    let params = mapPagedRequest(request)

    if (request.documentId) {
        params = params.append('DocumentId', request.documentId)
    }

    return params
}