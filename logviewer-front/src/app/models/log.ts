export class Log {
  ip: string;
  remoteAddr!: string;
  remoteUser: string;
  timeLocal!: string;
  requestUrl!: string;
  statusCode: string;
  bytesSent: string;
  httpRefer: string;
  httpUserAgent!: string;
  gzipRatio: string;

  constructor() {
  }
}
