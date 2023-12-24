import PagedRequest, { mapPagedRequest } from "../../PagedRequest";

export default interface GetUserSpaceRequest extends PagedRequest {
    ownerId?: number,
    statusGroupId?: number
}

export const mapGetUserSpaceRequest = (request: GetUserSpaceRequest) => {
    let params = mapPagedRequest(request)

    if(request.statusGroupId) {
        params = params.append('StatusGroupId', request.statusGroupId)
    }

    if(request.ownerId) {
        params = params.append('OwnerId', request.ownerId)
    }

    return params
}