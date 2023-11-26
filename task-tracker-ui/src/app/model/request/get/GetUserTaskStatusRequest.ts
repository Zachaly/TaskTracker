import PagedRequest, { mapPagedRequest } from "./PagedRequest"

export interface GetUserTaskStatusRequest extends PagedRequest {
    groupId?: number
    isDefault?: boolean
}

export const mapGetUserTaskStatusRequest = (request: GetUserTaskStatusRequest) => {
    let params = mapPagedRequest(request)

    if (request.isDefault != undefined) {
        params = params.append('IsDefault', request.isDefault)
    }

    if (request.groupId) {
        params = params.append('GroupId', request.groupId)
    }

    return params
}