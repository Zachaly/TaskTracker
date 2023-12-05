import { HttpParams } from "@angular/common/http"

export default interface PagedRequest {
    pageIndex?: number,
    pageSize?: number,
    skipPagination?: boolean,
    orderBy?: string
}

export const mapPagedRequest = (request: PagedRequest) : HttpParams => {
    let params = new HttpParams();

    if(request.pageIndex) {
        params = params.append('PageIndex', request.pageIndex)
    } 
    if(request.pageSize) {
        params = params.append('PageSize', request.pageSize)
    }
    if(request.skipPagination != undefined) {
        params = params.append('SkipPagination', request.skipPagination)
    }
    if(request.orderBy){
        params = params.append('OrderBy', request.orderBy)
    }

    return params;
}