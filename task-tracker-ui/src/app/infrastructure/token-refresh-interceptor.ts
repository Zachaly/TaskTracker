import { HTTP_INTERCEPTORS, HttpErrorResponse, HttpEvent, HttpHandler, HttpInterceptor, HttpRequest } from "@angular/common/http";
import { Observable, retry, tap } from "rxjs";
import { AuthService } from "../services/auth.service";
import { Injectable } from "@angular/core";
import { TokenService } from "../services/token.service";

@Injectable()
export class TokenRefreshInterceptor implements HttpInterceptor {
    constructor(private authService: AuthService, private tokenService: TokenService) {

    }

    intercept(req: HttpRequest<any>, next: HttpHandler): Observable<HttpEvent<any>> {
        req = req.clone({
            setHeaders: {
                'Authorization': `Bearer ${this.tokenService.getAccessToken()}`
            }
        })
        return next.handle(req).pipe(tap({
            error: async (err: HttpErrorResponse) => {
                if (err.status == 401 && !this.authService.isRefreshingToken) {
                    this.authService.refreshToken()?.subscribe(() => {
                        retry(1)
                    })
                }
                if(err.status == 403) {
                    alert("You are not allowed to do that!")
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