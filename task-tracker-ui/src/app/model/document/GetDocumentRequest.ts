import PagedRequest, { mapPagedRequest } from "../PagedRequest";

export default interface GetDocumentRequest extends PagedRequest {
    creatorId?: number,
    spaceId?: number
}

export const mapGetDocumentRequest = (request: GetDocumentRequest) => {
    let params = mapPagedRequest(request)

    if (request.creatorId) {
        params = params.append('CreatorId', request.creatorId)
    }

    if (request.spaceId) {
        params = params.append('SpaceId', request.spaceId)
    }

    return params
}