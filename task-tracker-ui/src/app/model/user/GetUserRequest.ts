import PagedRequest, { mapPagedRequest } from "../PagedRequest";

export default interface GetUserRequest extends PagedRequest {
    skipIds?: number[],
    searchEmail?: string
}

export const mapGetUserRequest = (request: GetUserRequest) => {
    let params = mapPagedRequest(request)

    if(request.skipIds) {
        request.skipIds.forEach(id => params = params.append('SkipIds', id))
    }

    if(request.searchEmail) {
        params = params.append('SearchEmail', request.searchEmail)
    }

    return params
}