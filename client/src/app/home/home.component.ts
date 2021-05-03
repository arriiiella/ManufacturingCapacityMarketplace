import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { ToastrService } from 'ngx-toastr';

@Component({
  selector: 'app-home',
  templateUrl: './home.component.html',
  styleUrls: ['./home.component.css'],
})
export class HomeComponent implements OnInit {
  constructor(private router: Router, public toastr: ToastrService) {}

  ngOnInit(): void {}

  clickRegister() {
    this.router.navigateByUrl('/register');
  }

  clickLogin() {
    this.router.navigateByUrl('/login');
  }
}
