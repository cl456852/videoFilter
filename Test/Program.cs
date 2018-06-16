using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BLL;
using DAL;
using System.IO;
using MODEL;
using System.Reflection;
using System.Collections;
using System.Text.RegularExpressions;

namespace Test
{
    class Program
    {
        static void Main(string[] args)
        {
            Program p = new Program();
            p.dictionaryTest();
            Console.Read();
        }

        void jsTest()
        {
           // var engine = new Jurassic.ScriptEngine();
            byte[] b = Encoding.Default.GetBytes("8.35.201.103");
            Console.WriteLine(Convert.ToBase64String(b));
        }

        void jsTest2()
        {
            string js = "eyJkYXRlIjoiMjAxNS0xMi0xMiIsInN0YXJ0Ijo2LCJ0b3RhbCI6IjgwIiwicm93cyI6W3siaWR4IjoiNDUzMzYiLCJzaXplIjoiMi4wNSIsImV4dGVuc2lvbiI6IldNViIsInZpZGVvX2lkIjoiU0NEMTQ1IiwiaGFzaCI6IjEzNWE0ZmQwOGU1MTJiZTYxZGM3OTY2ODZhNTI3NzBhIiwiYWN0cmVzcyI6IktPQkFZQVNISSBBU0FNSSIsImFjdHJlc3Nfc2VhcmNoIjoiNTkyNSJ9LHsiaWR4IjoiNDUzMzciLCJzaXplIjoiMi4xOCIsImV4dGVuc2lvbiI6IldNViIsInZpZGVvX2lkIjoiU0NEMTQ2IiwiaGFzaCI6IjFiOTE3ZjkyZGM3YThiZTExMGVmNmQzOGQ5YjQ4NTc1IiwiYWN0cmVzcyI6IktJVEFHQVdBIEVSSUtBIiwiYWN0cmVzc19zZWFyY2giOiI4OTkifSx7ImlkeCI6IjQ1MzM4Iiwic2l6ZSI6IjIuNTQiLCJleHRlbnNpb24iOiJNUDQiLCJ2aWRlb19pZCI6Ik9LU04yNjAiLCJoYXNoIjoiNmQyMTIwYTg1Y2NmOGJhYzI4YTMzMDUzYTliNzcxYWYiLCJhY3RyZXNzIjoiSklOIFlVS0kiLCJhY3RyZXNzX3NlYXJjaCI6IjExNjUifSx7ImlkeCI6IjQ1MzM5Iiwic2l6ZSI6IjMuMTciLCJleHRlbnNpb24iOiJNUDQiLCJ2aWRlb19pZCI6Ik1WU0QyODAiLCJoYXNoIjoiNzhjYjNhMDYyYWMwZjA2MjJkZGYzNjEwNTZlZTI1ZTQiLCJhY3RyZXNzIjoiQU5OTyBZVU1JIiwiYWN0cmVzc19zZWFyY2giOiIzNjEyIn0seyJpZHgiOiI0NTM0MCIsInNpemUiOiIyLjQ1IiwiZXh0ZW5zaW9uIjoiV01WIiwidmlkZW9faWQiOiJISU1BODEiLCJoYXNoIjoiNTA1YmM4ZDQ1YzJlZmM4N2JkNmUyNzFlYjQ0NjJjNzkiLCJhY3RyZXNzIjoiU0hJTk9NSVlBIENISUFLSSIsImFjdHJlc3Nfc2VhcmNoIjoiNjc4OSJ9LHsiaWR4IjoiNDUzNDEiLCJzaXplIjoiMS45OCIsImV4dGVuc2lvbiI6IldNViIsInZpZGVvX2lkIjoiSE9ORTE4NyIsImhhc2giOiJhODE0NTRhYTYzN2U2OGRiNjY0Y2Q5YzU2MjZlZjQyZiIsImFjdHJlc3MiOiJBSUhBUkEgSEFSVU5BIiwiYWN0cmVzc19zZWFyY2giOiI2MzAwIn0seyJpZHgiOiI0NTM0MiIsInNpemUiOiIzLjM1IiwiZXh0ZW5zaW9uIjoiTVA0IiwidmlkZW9faWQiOiJDUlBSNDQzIiwiaGFzaCI6IjI3NWUwNzIzNzg5NzNjNDNkY2QwYzQ3NzUzMDBiNTQ4IiwiYWN0cmVzcyI6Ik1BSVNBS0kgTUlLVU5JIiwiYWN0cmVzc19zZWFyY2giOiIxMjcifSx7ImlkeCI6IjQ1MzQzIiwic2l6ZSI6IjIuNTciLCJleHRlbnNpb24iOiJNUDQiLCJ2aWRlb19pZCI6IkhaMTAyOCIsImhhc2giOiJlMTc0M2E5NGM1NmE5NDlhNDBlZmI0YThkMzQyZDNmOCIsImFjdHJlc3MiOiJLT0pJTUEgS0FPUlUiLCJhY3RyZXNzX3NlYXJjaCI6IjQzNDIifSx7ImlkeCI6IjQ1MzQ0Iiwic2l6ZSI6IjQuMzciLCJleHRlbnNpb24iOiJNUDQiLCJ2aWRlb19pZCI6IkxBRkJENjYiLCJoYXNoIjoiZDkxMTg3ZTA3OWIyMGVmZDcyYWE1NzQ4OTZhMWY3OTgiLCJhY3RyZXNzIjoiS0FFREUgWVVLQSIsImFjdHJlc3Nfc2VhcmNoIjoiMzU5NyJ9LHsiaWR4IjoiNDUzNDUiLCJzaXplIjoiNC4zNiIsImV4dGVuc2lvbiI6Ik1QNCIsInZpZGVvX2lkIjoiTUtCRFMxMTYiLCJoYXNoIjoiZWI2MTg1MmVkMTc1NTE4MmY0MjMyY2VjZTYyZWIxNDgiLCJhY3RyZXNzIjoiU1VaVUhBIE1JVSIsImFjdHJlc3Nfc2VhcmNoIjoiMTg4OSJ9XSwiY3VybW9udGgiOiIyMDE1LTEyIiwicHJldm1vbnRoIjoiMjAxNS0xMSIsInRvcF9hY3RyZXNzZXMiOlt7ImlkeCI6Ijc2IiwibmFtZSI6IkFTVUtBIEtJUkFSQSIsInVybCI6Imh0dHA6XC9cL3BpY3MuZG1tLmNvLmpwXC9tb25vXC9hY3RqcGdzXC9tZWRpdW1cL2FzdWthX2tpcmFyYS5qcGciLCJ0b3RhbCI6IjEzMzk5IiwiZGF0ZSI6IjIwMTUtMTIifSx7ImlkeCI6IjE4NyIsIm5hbWUiOiJIQVRBTk8gWVVJIiwidXJsIjoiaHR0cDpcL1wvcGljcy5kbW0uY28uanBcL21vbm9cL2FjdGpwZ3NcL21lZGl1bVwvaGF0YW5vX3l1aS5qcGciLCJ0b3RhbCI6Ijk4NTciLCJkYXRlIjoiMjAxNS0xMiJ9LHsiaWR4IjoiMjkwOCIsIm5hbWUiOiJTSElOT0RBIEFZVU1JIiwidXJsIjoiaHR0cDpcL1wvcGljcy5kbW0uY28uanBcL21vbm9cL2FjdGpwZ3NcL21lZGl1bVwvc2lub2RhX2F5dW1pLmpwZyIsInRvdGFsIjoiODQ2OSIsImRhdGUiOiIyMDE1LTEyIn0seyJpZHgiOiI0NzE2IiwibmFtZSI6IlNISUJVWUEgS0FITyIsInVybCI6Imh0dHA6XC9cL3BpY3MuZG1tLmNvLmpwXC9tb25vXC9hY3RqcGdzXC9tZWRpdW1cL3NpYnV5YV9rYWhvLmpwZyIsInRvdGFsIjoiODIzNyIsImRhdGUiOiIyMDE1LTEyIn0seyJpZHgiOiI3NDY5IiwibmFtZSI6IlJJT04gIiwidXJsIjoiaHR0cDpcL1wvcGljcy5kbW0uY28uanBcL21vbm9cL2FjdGpwZ3NcL21lZGl1bVwvcmlvbjMuanBnIiwidG90YWwiOiI3Mjg3IiwiZGF0ZSI6IjIwMTUtMTIifSx7ImlkeCI6IjE0MDQiLCJuYW1lIjoiQVlBTUkgU1lVTktBIiwidXJsIjoiaHR0cDpcL1wvcGljcy5kbW0uY28uanBcL21vbm9cL2FjdGpwZ3NcL21lZGl1bVwvYXlhbWlfc3l1bmthLmpwZyIsInRvdGFsIjoiNzIxMCIsImRhdGUiOiIyMDE1LTEyIn0seyJpZHgiOiIzNDMiLCJuYW1lIjoiSEFNQVNBS0kgTUFPIiwidXJsIjoiaHR0cDpcL1wvcGljcy5kbW0uY28uanBcL21vbm9cL2FjdGpwZ3NcL21lZGl1bVwvaGFtYXNha2lfbWFvLmpwZyIsInRvdGFsIjoiNjQ3MiIsImRhdGUiOiIyMDE1LTEyIn0seyJpZHgiOiI2OTAiLCJuYW1lIjoiTUlaVU5PIEFTQUhJIiwidXJsIjoiaHR0cDpcL1wvcGljcy5kbW0uY28uanBcL21vbm9cL2FjdGpwZ3NcL21lZGl1bVwvbWl6dW5vX2FzYWhpLmpwZyIsInRvdGFsIjoiNjM0NSIsImRhdGUiOiIyMDE1LTEyIn0seyJpZHgiOiI0OTIiLCJuYW1lIjoiTUFUU1VNT1RPIE1FSSIsInVybCI6Imh0dHA6XC9cL3BpY3MuZG1tLmNvLmpwXC9tb25vXC9hY3RqcGdzXC9tZWRpdW1cL21hdHVtb3RvX21laS5qcGciLCJ0b3RhbCI6IjYzMDEiLCJkYXRlIjoiMjAxNS0xMiJ9LHsiaWR4IjoiMzc1MyIsIm5hbWUiOiJBTUFUU1VLQSBNT0UiLCJ1cmwiOiJodHRwOlwvXC9waWNzLmRtbS5jby5qcFwvbW9ub1wvYWN0anBnc1wvbWVkaXVtXC9hbWF0dWthX21vZS5qcGciLCJ0b3RhbCI6IjYxMTgiLCJkYXRlIjoiMjAxNS0xMiJ9XSwibmV3X2NvbnRlbnQiOlt7ImRhdGUiOiIyMDE1LTEyLTE3In0seyJkYXRlIjoiMjAxNS0xMi0xNiJ9LHsiZGF0ZSI6IjIwMTUtMTItMTUifSx7ImRhdGUiOiIyMDE1LTEyLTE0In0seyJkYXRlIjoiMjAxNS0xMi0xMyJ9LHsiZGF0ZSI6IjIwMTUtMTItMTIifSx7ImRhdGUiOiIyMDE1LTEyLTExIn1dLCJ0YWdzIjpbeyJpZHgiOiI4Iiwic2x1ZyI6ImFtYXRldXIiLCJuYW1lIjoiQU1BVEVVUiIsImRlc2NyaXB0aW9uIjoiIiwidXJsIjoiXC9pbWFnZXNcL3RhZ3NcL2FtYXRldXIucG5nIn0seyJpZHgiOiI1MyIsInNsdWciOiJhbWVyaWNhbiIsIm5hbWUiOiJBTUVSSUNBTiIsImRlc2NyaXB0aW9uIjoiIiwidXJsIjoiXC9pbWFnZXNcL3RhZ3NcL2FtZXJpY2FuLnBuZyJ9LHsiaWR4IjoiMzIiLCJzbHVnIjoiYW5hbCIsIm5hbWUiOiJBTkFMIiwiZGVzY3JpcHRpb24iOiIiLCJ1cmwiOiJcL2ltYWdlc1wvdGFnc1wvYW5hbC5wbmcifSx7ImlkeCI6IjQ4Iiwic2x1ZyI6ImFzcyIsIm5hbWUiOiJBU1MiLCJkZXNjcmlwdGlvbiI6IiIsInVybCI6IlwvaW1hZ2VzXC90YWdzXC9hc3MucG5nIn0seyJpZHgiOiI1OSIsInNsdWciOiJiZWFjaCIsIm5hbWUiOiJCRUFDSCIsImRlc2NyaXB0aW9uIjoiIiwidXJsIjoiXC9pbWFnZXNcL3RhZ3NcL2JlYWNoLnBuZyJ9LHsiaWR4IjoiNiIsInNsdWciOiJiZXN0IiwibmFtZSI6IkJFU1QiLCJkZXNjcmlwdGlvbiI6IiIsInVybCI6IlwvaW1hZ2VzXC90YWdzXC9iZXN0LnBuZyJ9LHsiaWR4IjoiMjMiLCJzbHVnIjoiYmlnLWJyZWFzdCIsIm5hbWUiOiJCSUctQlJFQVNUIiwiZGVzY3JpcHRpb24iOiIiLCJ1cmwiOiJcL2ltYWdlc1wvdGFnc1wvYmlnLWJyZWFzdC5wbmcifSx7ImlkeCI6IjUwIiwic2x1ZyI6ImJsYWNrIiwibmFtZSI6IkJMQUNLIiwiZGVzY3JpcHRpb24iOiIiLCJ1cmwiOiJcL2ltYWdlc1wvdGFnc1wvYmxhY2sucG5nIn0seyJpZHgiOiIyOCIsInNsdWciOiJib25kYWdlIiwibmFtZSI6IkJPTkRBR0UiLCJkZXNjcmlwdGlvbiI6IiIsInVybCI6IlwvaW1hZ2VzXC90YWdzXC9ib25kYWdlLnBuZyJ9LHsiaWR4IjoiMjYiLCJzbHVnIjoiYnVra2FrZSIsIm5hbWUiOiJCVUtLQUtFIiwiZGVzY3JpcHRpb24iOiIiLCJ1cmwiOiJcL2ltYWdlc1wvdGFnc1wvYnVra2FrZS5wbmcifSx7ImlkeCI6IjI5Iiwic2x1ZyI6ImJ1cyIsIm5hbWUiOiJCVVMiLCJkZXNjcmlwdGlvbiI6IiIsInVybCI6IlwvaW1hZ2VzXC90YWdzXC9idXMucG5nIn0seyJpZHgiOiI3NSIsInNsdWciOiJjYXIiLCJuYW1lIjoiQ0FSIiwiZGVzY3JpcHRpb24iOiIiLCJ1cmwiOiJcL2ltYWdlc1wvdGFnc1wvY2FyLnBuZyJ9LHsiaWR4IjoiNjUiLCJzbHVnIjoiY2xlYW5pbmctbGFkeSIsIm5hbWUiOiJDTEVBTklORy1MQURZIiwiZGVzY3JpcHRpb24iOiIiLCJ1cmwiOiJcL2ltYWdlc1wvdGFnc1wvY2xlYW5pbmctbGFkeS5wbmcifSx7ImlkeCI6IjE4Iiwic2x1ZyI6ImNvbGxlZ2UtZ2lybCIsIm5hbWUiOiJDT0xMRUdFLUdJUkwiLCJkZXNjcmlwdGlvbiI6IiIsInVybCI6IlwvaW1hZ2VzXC90YWdzXC9jb2xsZWdlLWdpcmwucG5nIn0seyJpZHgiOiI1Iiwic2x1ZyI6ImNvc3BsYXkiLCJuYW1lIjoiQ09TUExBWSIsImRlc2NyaXB0aW9uIjoiIiwidXJsIjoiXC9pbWFnZXNcL3RhZ3NcL2Nvc3BsYXkucG5nIn0seyJpZHgiOiIzIiwic2x1ZyI6ImNyZWFtcGllIiwibmFtZSI6IkNSRUFNUElFIiwiZGVzY3JpcHRpb24iOiIiLCJ1cmwiOiJcL2ltYWdlc1wvdGFnc1wvY3JlYW1waWUucG5nIn0seyJpZHgiOiI0NCIsInNsdWciOiJkYW5jZSIsIm5hbWUiOiJEQU5DRSIsImRlc2NyaXB0aW9uIjoiIiwidXJsIjoiXC9pbWFnZXNcL3RhZ3NcL2RhbmNlLnBuZyJ9LHsiaWR4IjoiMjEiLCJzbHVnIjoiZGVidXQiLCJuYW1lIjoiREVCVVQiLCJkZXNjcmlwdGlvbiI6IiIsInVybCI6IlwvaW1hZ2VzXC90YWdzXC9kZWJ1dC5wbmcifSx7ImlkeCI6IjY0Iiwic2x1ZyI6ImRvY3RvciIsIm5hbWUiOiJET0NUT1IiLCJkZXNjcmlwdGlvbiI6IiIsInVybCI6IlwvaW1hZ2VzXC90YWdzXC9kb2N0b3IucG5nIn0seyJpZHgiOiI2NiIsInNsdWciOiJkb3VibGUtcGVuZXRyYXRpb24iLCJuYW1lIjoiRE9VQkxFLVBFTkVUUkFUSU9OIiwiZGVzY3JpcHRpb24iOiIiLCJ1cmwiOiJcL2ltYWdlc1wvdGFnc1wvZG91YmxlLXBlbmV0cmF0aW9uLnBuZyJ9LHsiaWR4IjoiNTIiLCJzbHVnIjoiZHJ1Z2dlZCIsIm5hbWUiOiJEUlVHR0VEIiwiZGVzY3JpcHRpb24iOiIiLCJ1cmwiOiJcL2ltYWdlc1wvdGFnc1wvZHJ1Z2dlZC5wbmcifSx7ImlkeCI6IjIwIiwic2x1ZyI6ImZlbGxhdGlvIiwibmFtZSI6IkZFTExBVElPIiwiZGVzY3JpcHRpb24iOiIiLCJ1cmwiOiJcL2ltYWdlc1wvdGFnc1wvZmVsbGF0aW8ucG5nIn0seyJpZHgiOiI2OSIsInNsdWciOiJmaXN0IiwibmFtZSI6IkZJU1QiLCJkZXNjcmlwdGlvbiI6IiIsInVybCI6IlwvaW1hZ2VzXC90YWdzXC9maXN0LnBuZyJ9LHsiaWR4IjoiMzkiLCJzbHVnIjoiZ2FsIiwibmFtZSI6IkdBTCIsImRlc2NyaXB0aW9uIjoiIiwidXJsIjoiXC9pbWFnZXNcL3RhZ3NcL2dhbC5wbmcifSx7ImlkeCI6IjM0Iiwic2x1ZyI6ImdsYXNzZXMiLCJuYW1lIjoiR0xBU1NFUyIsImRlc2NyaXB0aW9uIjoiIiwidXJsIjoiXC9pbWFnZXNcL3RhZ3NcL2dsYXNzZXMucG5nIn0seyJpZHgiOiI4MyIsInNsdWciOiJncmF2dXJlIiwibmFtZSI6IkdSQVZVUkUiLCJkZXNjcmlwdGlvbiI6IiIsInVybCI6IlwvaW1hZ2VzXC90YWdzXC9ncmF2dXJlLnBuZyJ9LHsiaWR4IjoiNDMiLCJzbHVnIjoiZ3JvcGUiLCJuYW1lIjoiR1JPUEUiLCJkZXNjcmlwdGlvbiI6IiIsInVybCI6IlwvaW1hZ2VzXC90YWdzXC9ncm9wZS5wbmcifSx7ImlkeCI6IjY3Iiwic2x1ZyI6Imdyb3VwIiwibmFtZSI6IkdST1VQIiwiZGVzY3JpcHRpb24iOiIiLCJ1cmwiOiJcL2ltYWdlc1wvdGFnc1wvZ3JvdXAucG5nIn0seyJpZHgiOiI0Iiwic2x1ZyI6ImhhbmRqb2IiLCJuYW1lIjoiSEFOREpPQiIsImRlc2NyaXB0aW9uIjoiIiwidXJsIjoiXC9pbWFnZXNcL3RhZ3NcL2hhbmRqb2IucG5nIn0seyJpZHgiOiI1OCIsInNsdWciOiJoZW50YWkiLCJuYW1lIjoiSEVOVEFJIiwiZGVzY3JpcHRpb24iOiIiLCJ1cmwiOiJcL2ltYWdlc1wvdGFnc1wvaGVudGFpLnBuZyJ9LHsiaWR4IjoiNDciLCJzbHVnIjoiaW5jZXN0IiwibmFtZSI6IklOQ0VTVCIsImRlc2NyaXB0aW9uIjoiIiwidXJsIjoiXC9pbWFnZXNcL3RhZ3NcL2luY2VzdC5wbmcifSx7ImlkeCI6IjE2Iiwic2x1ZyI6Iml2IiwibmFtZSI6IklWIiwiZGVzY3JpcHRpb24iOiIiLCJ1cmwiOiJcL2ltYWdlc1wvdGFnc1wvaXYucG5nIn0seyJpZHgiOiI0NSIsInNsdWciOiJqayIsIm5hbWUiOiJKSyIsImRlc2NyaXB0aW9uIjoiIiwidXJsIjoiXC9pbWFnZXNcL3RhZ3NcL0pLLnBuZyJ9LHsiaWR4IjoiMjQiLCJzbHVnIjoia2ltb25vIiwibmFtZSI6IktJTU9OTyIsImRlc2NyaXB0aW9uIjoiIiwidXJsIjoiXC9pbWFnZXNcL3RhZ3NcL2tpbW9uby5wbmcifSx7ImlkeCI6IjYwIiwic2x1ZyI6ImxlZ3MiLCJuYW1lIjoiTEVHUyIsImRlc2NyaXB0aW9uIjoiIiwidXJsIjoiXC9pbWFnZXNcL3RhZ3NcL2xlZ3MucG5nIn0seyJpZHgiOiI0NiIsInNsdWciOiJsZXNiaWFuIiwibmFtZSI6IkxFU0JJQU4iLCJkZXNjcmlwdGlvbiI6IiIsInVybCI6IlwvaW1hZ2VzXC90YWdzXC9sZXNiaWFuLnBuZyJ9LHsiaWR4IjoiMSIsInNsdWciOiJsb2xpdGEiLCJuYW1lIjoiTE9MSVRBIiwiZGVzY3JpcHRpb24iOiIiLCJ1cmwiOiJcL2ltYWdlc1wvdGFnc1wvbG9saXRhLnBuZyJ9LHsiaWR4IjoiNzkiLCJzbHVnIjoibWFnbnVtIiwibmFtZSI6Ik1BR05VTSIsImRlc2NyaXB0aW9uIjoiIiwidXJsIjoiXC9pbWFnZXNcL3RhZ3NcL21hZ251bS5wbmcifSx7ImlkeCI6IjczIiwic2x1ZyI6Im1haWQiLCJuYW1lIjoiTUFJRCIsImRlc2NyaXB0aW9uIjoiIiwidXJsIjoiXC9pbWFnZXNcL3RhZ3NcL21haWQucG5nIn0seyJpZHgiOiIzMCIsInNsdWciOiJtYXNzYWdlIiwibmFtZSI6Ik1BU1NBR0UiLCJkZXNjcmlwdGlvbiI6IiIsInVybCI6IlwvaW1hZ2VzXC90YWdzXC9tYXNzYWdlLnBuZyJ9LHsiaWR4IjoiMTUiLCJzbHVnIjoibWF0dXJlIiwibmFtZSI6Ik1BVFVSRSIsImRlc2NyaXB0aW9uIjoiIiwidXJsIjoiXC9pbWFnZXNcL3RhZ3NcL21hdHVyZS5wbmcifSx7ImlkeCI6IjU1Iiwic2x1ZyI6Im1pbGsiLCJuYW1lIjoiTUlMSyIsImRlc2NyaXB0aW9uIjoiIiwidXJsIjoiXC9pbWFnZXNcL3RhZ3NcL21pbGsucG5nIn0seyJpZHgiOiI0MCIsInNsdWciOiJudXJzZSIsIm5hbWUiOiJOVVJTRSIsImRlc2NyaXB0aW9uIjoiIiwidXJsIjoiXC9pbWFnZXNcL3RhZ3NcL251cnNlLnBuZyJ9LHsiaWR4IjoiNDIiLCJzbHVnIjoib2wiLCJuYW1lIjoiT0wiLCJkZXNjcmlwdGlvbiI6IiIsInVybCI6IlwvaW1hZ2VzXC90YWdzXC9PTC5wbmcifSx7ImlkeCI6IjEyIiwic2x1ZyI6Im9uYW5pZSIsIm5hbWUiOiJPTkFOSUUiLCJkZXNjcmlwdGlvbiI6IiIsInVybCI6IlwvaW1hZ2VzXC90YWdzXC9vbmFuaWUucG5nIn0seyJpZHgiOiI1NyIsInNsdWciOiJvbnNlbiIsIm5hbWUiOiJPTlNFTiIsImRlc2NyaXB0aW9uIjoiIiwidXJsIjoiXC9pbWFnZXNcL3RhZ3NcL29uc2VuLnBuZyJ9LHsiaWR4IjoiMTMiLCJzbHVnIjoib3V0ZG9vciIsIm5hbWUiOiJPVVRET09SIiwiZGVzY3JpcHRpb24iOiIiLCJ1cmwiOiJcL2ltYWdlc1wvdGFnc1wvb3V0ZG9vci5wbmcifSx7ImlkeCI6IjM2Iiwic2x1ZyI6InBhaXp1cmkiLCJuYW1lIjoiUEFJWlVSSSIsImRlc2NyaXB0aW9uIjoiIiwidXJsIjoiXC9pbWFnZXNcL3RhZ3NcL3BhaXp1cmkucG5nIn0seyJpZHgiOiI3NyIsInNsdWciOiJwcmVnbmFudCIsIm5hbWUiOiJQUkVHTkFOVCIsImRlc2NyaXB0aW9uIjoiIiwidXJsIjoiXC9pbWFnZXNcL3RhZ3NcL3ByZWduYW50LnBuZyJ9LHsiaWR4IjoiMjIiLCJzbHVnIjoicHJvc3RpdHV0ZSIsIm5hbWUiOiJQUk9TVElUVVRFIiwiZGVzY3JpcHRpb24iOiIiLCJ1cmwiOiJcL2ltYWdlc1wvdGFnc1wvcHJvc3RpdHV0ZS5wbmcifSx7ImlkeCI6IjExIiwic2x1ZyI6InB1YmxpYyIsIm5hbWUiOiJQVUJMSUMiLCJkZXNjcmlwdGlvbiI6IiIsInVybCI6IlwvaW1hZ2VzXC90YWdzXC9wdWJsaWMucG5nIn0seyJpZHgiOiI2MSIsInNsdWciOiJyYWNlLXF1ZWVuIiwibmFtZSI6IlJBQ0UtUVVFRU4iLCJkZXNjcmlwdGlvbiI6IiIsInVybCI6IlwvaW1hZ2VzXC90YWdzXC9yYWNlLXF1ZWVuLnBuZyJ9LHsiaWR4IjoiMzciLCJzbHVnIjoicmFwZSIsIm5hbWUiOiJSQVBFIiwiZGVzY3JpcHRpb24iOiIiLCJ1cmwiOiJcL2ltYWdlc1wvdGFnc1wvcmFwZS5wbmcifSx7ImlkeCI6Ijg0Iiwic2x1ZyI6InNhbnRhIiwibmFtZSI6IlNBTlRBIiwiZGVzY3JpcHRpb24iOiIiLCJ1cmwiOiJcL2ltYWdlc1wvdGFnc1wvc2FudGEucG5nIn0seyJpZHgiOiI5Iiwic2x1ZyI6InNjaG9vbC1naXJsIiwibmFtZSI6IlNDSE9PTC1HSVJMIiwiZGVzY3JpcHRpb24iOiIiLCJ1cmwiOiJcL2ltYWdlc1wvdGFnc1wvc2Nob29sLWdpcmwucG5nIn0seyJpZHgiOiI3NCIsInNsdWciOiJzZWUtdGhydSIsIm5hbWUiOiJTRUUtVEhSVSIsImRlc2NyaXB0aW9uIjoiIiwidXJsIjoiXC9pbWFnZXNcL3RhZ3NcL3NlZS10aHJ1LnBuZyJ9LHsiaWR4IjoiMzgiLCJzbHVnIjoic2hhdmVkIiwibmFtZSI6IlNIQVZFRCIsImRlc2NyaXB0aW9uIjoiIiwidXJsIjoiXC9pbWFnZXNcL3RhZ3NcL3NoYXZlZC5wbmcifSx7ImlkeCI6IjYzIiwic2x1ZyI6InNoZS1tYWxlIiwibmFtZSI6IlNIRS1NQUxFIiwiZGVzY3JpcHRpb24iOiIiLCJ1cmwiOiJcL2ltYWdlc1wvdGFnc1wvc2hlLW1hbGUucG5nIn0seyJpZHgiOiI4MiIsInNsdWciOiJzbGVlcCIsIm5hbWUiOiJTTEVFUCIsImRlc2NyaXB0aW9uIjoiIiwidXJsIjoiXC9pbWFnZXNcL3RhZ3NcL3NsZWVwLnBuZyJ9LHsiaWR4IjoiMzEiLCJzbHVnIjoic2xpbSIsIm5hbWUiOiJTTElNIiwiZGVzY3JpcHRpb24iOiIiLCJ1cmwiOiJcL2ltYWdlc1wvdGFnc1wvc2xpbS5wbmcifSx7ImlkeCI6IjI3Iiwic2x1ZyI6InNtIiwibmFtZSI6IlNNIiwiZGVzY3JpcHRpb24iOiIiLCJ1cmwiOiJcL2ltYWdlc1wvdGFnc1wvc20ucG5nIn0seyJpZHgiOiI1NCIsInNsdWciOiJzb2FwbGFuZCIsIm5hbWUiOiJTT0FQTEFORCIsImRlc2NyaXB0aW9uIjoiIiwidXJsIjoiXC9pbWFnZXNcL3RhZ3NcL3NvYXBsYW5kLnBuZyJ9LHsiaWR4IjoiNzgiLCJzbHVnIjoic3BvcnRzIiwibmFtZSI6IlNQT1JUUyIsImRlc2NyaXB0aW9uIjoiIiwidXJsIjoiXC9pbWFnZXNcL3RhZ3NcL3Nwb3J0cy5wbmcifSx7ImlkeCI6IjM1Iiwic2x1ZyI6InNxdWlydCIsIm5hbWUiOiJTUVVJUlQiLCJkZXNjcmlwdGlvbiI6IiIsInVybCI6IlwvaW1hZ2VzXC90YWdzXC9zcXVpcnQucG5nIn0seyJpZHgiOiI1NiIsInNsdWciOiJzd2FsbG93IiwibmFtZSI6IlNXQUxMT1ciLCJkZXNjcmlwdGlvbiI6IiIsInVybCI6IlwvaW1hZ2VzXC90YWdzXC9zd2FsbG93LnBuZyJ9LHsiaWR4IjoiMiIsInNsdWciOiJzd2ltc3VpdCIsIm5hbWUiOiJTV0lNU1VJVCIsImRlc2NyaXB0aW9uIjoiIiwidXJsIjoiXC9pbWFnZXNcL3RhZ3NcL3N3aW1zdWl0LnBuZyJ9LHsiaWR4IjoiMzMiLCJzbHVnIjoidGVhY2hlciIsIm5hbWUiOiJURUFDSEVSIiwiZGVzY3JpcHRpb24iOiIiLCJ1cmwiOiJcL2ltYWdlc1wvdGFnc1wvdGVhY2hlci5wbmcifSx7ImlkeCI6IjcyIiwic2x1ZyI6InRlbnRhY2xlIiwibmFtZSI6IlRFTlRBQ0xFIiwiZGVzY3JpcHRpb24iOiIiLCJ1cmwiOiJcL2ltYWdlc1wvdGFnc1wvdGVudGFjbGUucG5nIn0seyJpZHgiOiI1MSIsInNsdWciOiJ0aWdodHMiLCJuYW1lIjoiVElHSFRTIiwiZGVzY3JpcHRpb24iOiIiLCJ1cmwiOiJcL2ltYWdlc1wvdGFnc1wvdGlnaHRzLnBuZyJ9LHsiaWR4IjoiNjIiLCJzbHVnIjoidG9ydHVyZSIsIm5hbWUiOiJUT1JUVVJFIiwiZGVzY3JpcHRpb24iOiIiLCJ1cmwiOiJcL2ltYWdlc1wvdGFnc1wvdG9ydHVyZS5wbmcifSx7ImlkeCI6IjEwIiwic2x1ZyI6InRyYWluIiwibmFtZSI6IlRSQUlOIiwiZGVzY3JpcHRpb24iOiIiLCJ1cmwiOiJcL2ltYWdlc1wvdGFnc1wvdHJhaW4ucG5nIn0seyJpZHgiOiIxNyIsInNsdWciOiJ1bmNlbnNvcmVkIiwibmFtZSI6IlVOQ0VOU09SRUQiLCJkZXNjcmlwdGlvbiI6IiIsInVybCI6IlwvaW1hZ2VzXC90YWdzXC91bmNlbnNvcmVkLnBuZyJ9LHsiaWR4IjoiMTQiLCJzbHVnIjoidXJpbmF0ZSIsIm5hbWUiOiJVUklOQVRFIiwiZGVzY3JpcHRpb24iOiIiLCJ1cmwiOiJcL2ltYWdlc1wvdGFnc1wvdXJpbmF0ZS5wbmcifSx7ImlkeCI6IjcwIiwic2x1ZyI6InZpcmdpbiIsIm5hbWUiOiJWSVJHSU4iLCJkZXNjcmlwdGlvbiI6IiIsInVybCI6IlwvaW1hZ2VzXC90YWdzXC92aXJnaW4ucG5nIn0seyJpZHgiOiI0OSIsInNsdWciOiJ3LSIsIm5hbWUiOiJXLiIsImRlc2NyaXB0aW9uIjoiIiwidXJsIjoiXC9pbWFnZXNcL3RhZ3NcL1cucG5nIn0seyJpZHgiOiIyNSIsInNsdWciOiJ3ZWRkaW5nIiwibmFtZSI6IldFRERJTkciLCJkZXNjcmlwdGlvbiI6IiIsInVybCI6IlwvaW1hZ2VzXC90YWdzXC93ZWRkaW5nLnBuZyJ9LHsiaWR4IjoiNDEiLCJzbHVnIjoid2VpcmQiLCJuYW1lIjoiV0VJUkQiLCJkZXNjcmlwdGlvbiI6IiIsInVybCI6IlwvaW1hZ2VzXC90YWdzXC93ZWlyZC5wbmcifSx7ImlkeCI6IjE5Iiwic2x1ZyI6IndoaXRlIiwibmFtZSI6IldISVRFIiwiZGVzY3JpcHRpb24iOiIiLCJ1cmwiOiJcL2ltYWdlc1wvdGFnc1wvd2hpdGUucG5nIn0seyJpZHgiOiI3MSIsInNsdWciOiJ3aWRvdyIsIm5hbWUiOiJXSURPVyIsImRlc2NyaXB0aW9uIjoiIiwidXJsIjoiXC9pbWFnZXNcL3RhZ3NcL3dpZG93LnBuZyJ9LHsiaWR4IjoiNyIsInNsdWciOiJ3aWZlIiwibmFtZSI6IldJRkUiLCJkZXNjcmlwdGlvbiI6IiIsInVybCI6IlwvaW1hZ2VzXC90YWdzXC93aWZlLnBuZyJ9XX0=";
          //  var engine = new Jurassic.ScriptEngine();
            string res = Encoding.Default.GetString(Convert.FromBase64String(js));

        }

        void sortTest()
        {
            ArrayList list = new ArrayList();
            list.Add("a");
            list.Add("abc");
            list.Add("bac");
            list.Add("b");
            list.Add("4g");
            list.Add("a");
            list.Sort();
            foreach (string a in list)
                Console.WriteLine(a);
        }

        void dictTest()
        {
            SortedDictionary<string, string> dic = new SortedDictionary<string, string>();
            dic.Remove("ddd");
        }

        void updateVid()
        {
            Regex idRegex = new Regex("[A-Z]{1,}-[0-9]{1,}|[A-Z]{1,}[0-9]{1,}");
            List<MyFileInfo> list= FileDAL.selectMyFileInfo("");
            Regex reg1 = new Regex("[A-Z]");
            foreach (MyFileInfo myFileInfo in list)
            {
                if (myFileInfo.Length > 100)
                {
                    if(myFileInfo.FileName.Length>40&&!CheckChinese(myFileInfo.FileName))
                    {
                        continue;
                    }
                    string vid = idRegex.Match(Path.GetFileNameWithoutExtension(myFileInfo.FileName).ToUpper().Replace("-","")).ToString();
                  
                    if (vid.Length > 0)
                    {

                           string letter = "";
                           string number = "";
                           bool isEndofLetter = false;
                           for (int i = 0; i < vid.Length; i++)                        //修改   对于出现KIDM235A  KIDM235B
                            if (reg1.IsMatch(vid[i].ToString()))
                            {
                                 if (isEndofLetter)
                                    break;
                                 else
                                    letter += vid[i];
                            }
                            else
                            {
                                number += vid[i];                        
                                isEndofLetter = true;
                            }
                            if(letter.Length>6||letter.Length==1)
                                continue;
                            if(number.Length>8)
                                continue;
                            myFileInfo.Vid = vid;
                            FileDAL.UpdateVid(myFileInfo);
                       
                    }
                }

            }
            

        }

        bool CheckChinese(string s)
        {
            foreach (char c in s)
            {
                if (c >= 0x4E00 && c <= 0x9FA5)
                    return true;
                else if (c >= 0x3040 && c <= 0x309F)
                    return true;
                else if (c >= 0x30A0 && c <= 0x30FF)
                    return true;
            }
            return false;
        }

        void CmdTest()
        {
            string str="d:\\curl \"http://kikibt.net/\" -H \"Cookie: __cfduid=d84d7ed42600397ebc0617366fc2e02bc1477489552; a2204_times=13; CNZZDATA1260997767=915447713-1482412125-^%^7C1492937714; CNZZDATA1261675006=724777673-1492252758-^%^7C1492937311; UM_distinctid=15ed86daa300-0f615ded0eedfe-3a3e5c06-1fa400-15ed86daa34ba6; __atuvc=3^%^7C36^%^2C0^%^7C37^%^2C0^%^7C38^%^2C0^%^7C39^%^2C45^%^7C40; CNZZDATA1261857871=1481140133-1494768247-^%^7C1507030268; CNZZDATA1261841250=1785091964-1494766693-^%^7C1507028747; Hm_lvt_bd3d4db2c728324e870543c59e9e3b89=1504709377,1506869631; Hm_lpvt_bd3d4db2c728324e870543c59e9e3b89=1507031416; a5161_pages=1; a5161_times=10; Hm_lvt_f75b813e9c1ef4fb27eaa613c9f307b2=1504709378,1506869631; Hm_lpvt_f75b813e9c1ef4fb27eaa613c9f307b2=1507031416\" -H \"Origin: http://kikibt.net\" -H \"Accept-Encoding: gzip, deflate\" -H \"Accept-Language: zh-CN,zh;q=0.8,en;q=0.6,es;q=0.4\" -H \"Upgrade-Insecure-Requests: 1\" -H \"User-Agent: Mozilla/5.0 (Windows NT 6.3; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/60.0.3112.101 Safari/537.36\" -H \"Content-Type: application/x-www-form-urlencoded\" -H \"Accept: text/html,application/xhtml+xml,application/xml;q=0.9,image/webp,image/apng,*/*;q=0.8\" -H \"Cache-Control: max-age=0\" -H \"Referer: http://kikibt.net/search/e7Zl9_O5DQA/1-0-0.html\" -H \"Connection: keep-alive\" --data \"keyword=kidm 200\" --compressed -vi";
            System.Diagnostics.Process p = new System.Diagnostics.Process();
            p.StartInfo.FileName = "cmd.exe";
            p.StartInfo.UseShellExecute = false;    //是否使用操作系统shell启动
            p.StartInfo.RedirectStandardInput = true;//接受来自调用程序的输入信息
            p.StartInfo.RedirectStandardOutput = true;//由调用程序获取输出信息
            p.StartInfo.RedirectStandardError = true;//重定向标准错误输出
            p.StartInfo.CreateNoWindow = true;//不显示程序窗口
            p.Start();//启动程序

            //向cmd窗口发送输入信息
            p.StandardInput.WriteLine(str + "&exit");

            p.StandardInput.AutoFlush = true;
            //p.StandardInput.WriteLine("exit");
            //向标准输入写入要执行的命令。这里使用&是批处理命令的符号，表示前面一个命令不管是否执行成功都执行后面(exit)命令，如果不执行exit命令，后面调用ReadToEnd()方法会假死
            //同类的符号还有&&和||前者表示必须前一个命令执行成功才会执行后面的命令，后者表示必须前一个命令执行失败才会执行后面的命令



            //获取cmd窗口的输出信息
            string output = p.StandardOutput.ReadToEnd();
            string[] spliter = new string[] { "Location:", "\r\n" };
            string[] res = output.Split(spliter, StringSplitOptions.RemoveEmptyEntries);

            //StreamReader reader = p.StandardOutput;
            //string line=reader.ReadLine();
            //while (!reader.EndOfStream)
            //{
            //    str += line + "  ";
            //    line = reader.ReadLine();
            //}

       
            
            p.Close();


            Console.WriteLine(output);
        }

        public void dictionaryTest()
        {

            Dictionary<string, KikiDO> dictionarySearch = new Dictionary<string, KikiDO>();
            KikiDO kikiDO = new KikiDO();
            kikiDO.Url = "a";
            dictionarySearch.Add("a",kikiDO);
            dictionarySearch.Add("b", kikiDO);
            KikiDO k= dictionarySearch["a"];
            k.Url = "d";
            foreach (var item in dictionarySearch)

            {

                Console.WriteLine(item.Key +" "+ item.Value.Url);

            }



        }

    }
}
