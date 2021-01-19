import {Component, OnInit} from '@angular/core';
import {FormBuilder, FormControl, FormGroup, Validators} from '@angular/forms';
import {ActivatedRoute, Router} from '@angular/router';
import {LogsService} from '../../services/logs.service';
import {AlertService} from '../../services/alert.service';
import {first} from 'rxjs/operators';
import {IpVersion, RxwebValidators} from '@rxweb/reactive-form-validators';

@Component({
  selector: 'app-add-edit-logs',
  templateUrl: './add-edit-logs.component.html',
  styleUrls: ['./add-edit-logs.component.scss']
})
export class AddEditLogsComponent implements OnInit {
  isAddMode!: boolean;
  form!: FormGroup;
  id!: string;
  loading = false;
  submitted = false;

  // Test
  ipFormControl: FormControl;
  constructor(
    private formBuilder: FormBuilder,
    private route: ActivatedRoute,
    private router: Router,
    private logService: LogsService,
    private alertService: AlertService
  ) {}

  ngOnInit(): void {
    this.id = this.route.snapshot.params.id;
    this.isAddMode = !this.id;

    this.form = this.formBuilder.group({
      remoteAddr: ['', [Validators.required, RxwebValidators.ip({version: IpVersion.AnyOne})]],
      remoteUser: ['', Validators.nullValidator],
      timeLocal: ['', Validators.required],
      requestUrl: ['', Validators.required],
      statusCode: ['', Validators.required],
      bytesSent: ['', [Validators.nullValidator, RxwebValidators.digit]],
      httpRefer: [''],
      httpUserAgent: ['', Validators.required],
      gzipRatio: ['', [Validators.nullValidator, RxwebValidators.digit]]
    });

    this.ipFormControl = new FormControl('', [Validators.required]);

    if (!this.isAddMode){
      this.logService.getById(this.id)
        .pipe(first())
        .subscribe(x => this.form.patchValue(x));
    }
  }

  onSubmit(): void {
    this.submitted = true;
    this.alertService.clear();

    if (this.form.invalid){
      return;
    }

    this.loading = true;
    if (this.isAddMode) {
      this.createLog();
    } else {
      this.updateLog();
    }
  }

  private createLog(): void {
    this.logService.create(this.form.value)
      .pipe(first())
      .subscribe(() => {
        this.alertService.success('Log added', { keepAfterRouteChange: true});
        this.router.navigate(['../'], {relativeTo: this.route});
      }).add(() => this.loading = false);
  }

  private updateLog(): void {
    this.logService.update(this.id, this.form.value)
      .pipe(first())
      .subscribe(() => {
        this.alertService.success('Log updated', { keepAfterRouteChange: true});
        this.router.navigate(['../../'], {relativeTo: this.route});
      }).add(() => this.loading = false);
  }
}
