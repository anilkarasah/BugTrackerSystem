export interface Bug {
  id: string;
  title: string;
  description: string;
  status: string;
  projectID: string;
  projectName: string;
  reporterID: string;
  reporterName: string;
  createdAt: string;
  lastUpdatedAt: string;
}

export interface UpsertBug {
  title?: string;
  description?: string;
  status?: string;
}
