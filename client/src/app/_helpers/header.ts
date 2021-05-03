import { HttpInterceptor, HttpRequest, HttpHandler, HttpEvent } from "@angular/common/http";
import { Injectable } from "@angular/core";
import { Observable } from "rxjs";

@Injectable()
export class TokenInterceptor implements HttpInterceptor {

    intercept(req: HttpRequest<any>, next: HttpHandler): Observable<HttpEvent<any>> {
        var tokenInfo = localStorage.getItem('user');
        if (tokenInfo && tokenInfo != null) {
            const userToken = JSON.parse(tokenInfo);
            const modifiedReq = req.clone({
                headers: req.headers.set('Authorization', `Bearer ${userToken.token}`),
            });
            return next.handle(modifiedReq);
        } else {
            return next.handle(req);
        }
    }
}