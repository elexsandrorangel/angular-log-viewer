import { ComponentFixture, TestBed } from '@angular/core/testing';

import { LogsImportComponent } from './logs-import.component';
import {HttpClientTestingModule} from '@angular/common/http/testing';
import {MatInputModule} from '@angular/material/input';
import {MatTableModule} from '@angular/material/table';
import {MatPaginatorModule} from '@angular/material/paginator';
import {MatSortModule} from '@angular/material/sort';
import {MatFormFieldModule} from '@angular/material/form-field';
import {MatButtonModule} from '@angular/material/button';
import {MatIconModule} from '@angular/material/icon';
import {NgxDropzoneModule} from 'ngx-dropzone';
import {MatToolbarModule} from '@angular/material/toolbar';

describe('LogsImportComponent', () => {
  let component: LogsImportComponent;
  let fixture: ComponentFixture<LogsImportComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ LogsImportComponent ],
      imports: [
        HttpClientTestingModule,

        MatToolbarModule,
        MatInputModule,
        MatTableModule,
        MatPaginatorModule,
        MatSortModule,
        MatFormFieldModule,
        MatButtonModule,
        MatIconModule,
        NgxDropzoneModule
      ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(LogsImportComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
