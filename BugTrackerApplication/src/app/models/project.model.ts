import { BugReportData } from './bug.model';
import { ContributorData } from './user.model';

export interface Project {
  id: string;
  name: string;
  leaderName: string;
  contributorNamesList: ContributorData[];
  bugReportsList: BugReportData[];
}

export interface ProjectData {
  id: string;
  name: string;
}
