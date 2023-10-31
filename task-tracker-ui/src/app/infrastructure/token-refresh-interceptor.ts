import { HTTP_INTERCEPTORS, HttpErrorResponse, HttpEvent, HttpHandler, HttpInterceptor, HttpRequest } from "@angular/common/http";
import { Observable, retry, tap } from "rxjs";
import { AuthService } from "../services/auth.service";
import { Injectable } from "@angular/core";

@Injectable()
export class TokenRefreshInterceptor implements HttpInterceptor {
    constructor(private authService: AuthService) {

    }

    intercept(req: HttpRequest<any>, next: HttpHandler): Observable<HttpEvent<any>> {
        req = req.clone()

        req.headers.set('Authorization', `Bearer ${this.authService.userData?.accessToken}`)

        return next.handle(req).pipe(tap({
            error: async (err: HttpErrorResponse) => {
                console.log(err)
                if(err.status == 401 && !this.authService.isRefreshingToken) {
                    await this.authService.refreshToken()

                    if(this.authService.userData) {
                        retry(1)
                    }
                }
            }
        }))
    }
}


export const tokenInterceptor = {
    provide: HTTP_INTERCEPTORS,
    useClass: TokenRefreshInterceptor,
    multi: true
  };