import PagedRequest, { mapPagedRequest } from "../PagedRequest";

export default interface GetSpaceUserPermissionsRequest extends PagedRequest {
    userId?: number,
    spaceId?: number
}

export const mapGetSpaceUserPermissionsRequest = (request: GetSpaceUserPermissionsRequest) => {
    let params = mapPagedRequest(request)

    if(request.spaceId){
        params = params.append('SpaceId', request.spaceId)
    }

    if(request.userId) {
        params = params.append('UserId', request.userId)
    }

    return params
}