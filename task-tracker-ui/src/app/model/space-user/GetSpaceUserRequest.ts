import PagedRequest, { mapPagedRequest } from "../PagedRequest";

export default interface GetSpaceUserRequest extends PagedRequest {
    userId?: number,
    spaceId?: number
}

export const mapGetSpaceUserRequest = (request: GetSpaceUserRequest) => {
    let params = mapPagedRequest(request)

    if(request.userId) {
        params = params.append('UserId', request.userId)
    }

    if(request.spaceId) {
        params = params.append('SpaceId', request.spaceId)
    }

    return params
}