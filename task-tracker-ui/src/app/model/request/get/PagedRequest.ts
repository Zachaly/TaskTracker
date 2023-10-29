import { HttpParams } from "@angular/common/http"

export default interface PagedRequest {
    pageIndex?: number,
    pageSize?: number
}

export const mapPagedRequest = (request: PagedRequest) : HttpParams => {
    let params = new HttpParams();

    if(request.pageIndex) {
        params = params.append('PageIndex', request.pageIndex)
    } 
    if(request.pageSize) {
        params = params.append('PageSize', request.pageSize)
    }

    return params;
}