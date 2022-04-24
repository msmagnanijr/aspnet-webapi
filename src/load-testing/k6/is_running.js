import { check } from "k6";
import http from "k6/http";

export default function () {
    let res = http.get("https://localhost:3001/categories/");
    check(res, {
        "is status 200": (r) => r.status === 200
    });
};