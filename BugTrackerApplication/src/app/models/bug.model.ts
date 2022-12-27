export interface Bug {
  id: string;
  title: string;
  description: string;
  status: string;
  projectID: string;
  projectName: string;
  reporterID: string;
  reporterName: string;
  createdAt: Date;
  lastUpdatedAt: Date;
}

export interface UpsertBug {
  title?: string;
  description?: string;
  status?: string;
}

export interface BugReportData {
  id: Number;
  title: string;
}
