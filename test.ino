void
setup()
{
  Serial.begin(9600);
}

static char buffer[512] = {0};
static char *ptr = buffer;

static void
handle_command()
{
  int len = strlen(buffer);

  for(int i = len; i >= 0; --i)
  {
    Serial.print(buffer[i]);
  }
  
  Serial.println();
}

void
loop()
{
  while(Serial.available())
  {
    char c = Serial.read();

    if(c == '\n')
    {
      *ptr = '\0';
      handle_command();      
      ptr = buffer;
    }
    else
    {
      if(ptr - buffer < 512)
      {
        *ptr = c;
        ptr++;
      }
    }
  }
}
