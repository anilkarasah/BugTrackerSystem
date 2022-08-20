export default interface Bug {
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
